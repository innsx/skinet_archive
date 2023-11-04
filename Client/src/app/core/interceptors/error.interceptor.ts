import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { catchError, delay } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toastr: ToastrService) {}

  // catching any ERRORS that comes back as HTTP responses
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(error => {
        if (error.status === 400) {
          if (error.error.errors) {
            throw error.error;
          } else {
            this.toastr.error(error.error.message, error.error.statusCode);
          }
        }

         // if status code = 401, then use TOASTR POPUP with ERROR in HTTP RESPONSE
        if (error.status === 401) {
          this.toastr.error(error.error.message, error.error.statusCode);
        }

         // if status code = 404, then redirect by below URL
        if (error.status === 404){
            this.router.navigateByUrl('/not-found');
        }

        // if status code = 500, then redirect by below URL
        if (error.status === 500){
          const navigationExtras: NavigationExtras = {state: {error: error.error}};
          this.router.navigateByUrl('/server-error', navigationExtras);

        }
        // catching any OTHER ERRORS NOT FILTERED ABOVE BY THROWING THE ERROR
        return throwError(error);
      })
    );
  }
}

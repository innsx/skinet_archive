import { Component, OnInit } from '@angular/core';
import { IOrder } from 'src/app/shared/models/order';
import { OrdersService } from '../orders.service';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-order-detailed',
  templateUrl: './order-detailed.component.html',
  styleUrls: ['./order-detailed.component.scss']
})
export class OrderDetailedComponent implements OnInit {
  order: IOrder;

  constructor(private orderService: OrdersService, private activateRoute: ActivatedRoute, private breadcrumService: BreadcrumbService) {
    this.breadcrumService.set('@OrderDetailed', ''); // set and initialized breadcrumb alias to an empty string
  }

  ngOnInit(): void {
    this.getOrderDetailed();
  }

  getOrderDetailed() {
    const id: number = +this.activateRoute.snapshot.paramMap.get('id');

    this.orderService.getOrderDetailed(id).subscribe((response: IOrder) => {
      this.order = response;
      this.breadcrumService.set('@OrderDetailed', `Order# ${response.id} - ${response.status}`); // set breadcrumb string
    }, error => {
      console.log(error);
    });
  }
}

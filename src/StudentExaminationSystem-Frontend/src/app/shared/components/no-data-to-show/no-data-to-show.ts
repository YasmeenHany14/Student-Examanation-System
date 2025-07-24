import { Component, Input } from '@angular/core';
import {Card} from 'primeng/card';

@Component({
  selector: 'app-no-data-to-show',
  standalone: true,
  templateUrl: './no-data-to-show.html',
  imports: [
    Card
  ],
  styleUrls: ['./no-data-to-show.scss']
})
export class NoDataToShowComponent {
  @Input() message?: string;
}


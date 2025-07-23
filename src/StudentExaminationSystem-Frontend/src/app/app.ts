import {RouterOutlet} from '@angular/router';
import {Component} from '@angular/core';
import {ToastModule} from 'primeng/toast';
import {Header} from './layout/header/header';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ToastModule, Header],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class AppComponent {
  title = 'Student Examination System';
}

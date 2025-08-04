import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MessageModule } from 'primeng/message';

@Component({
  selector: 'app-error-message',
  standalone: true,
  imports: [CommonModule, MessageModule],
  templateUrl: './error-message.html',
  styleUrls: ['./error-message.scss']
})
export class ErrorMessageComponent {
  @Input() message: string = 'An error occurred';
  @Input() severity: 'error' | 'warn' | 'info' | 'success' = 'error';
  @Input() closable: boolean = false;
  @Input() showIcon: boolean = true;
}

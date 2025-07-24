import { Component, Input, Output, EventEmitter } from '@angular/core';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-delete-confirmation-dialog',
  templateUrl: './delete-confirmation-dialog.html',
  styleUrls: ['./delete-confirmation-dialog.scss'],
  standalone: true,
  imports: [DialogModule, ButtonModule]
})
export class DeleteConfirmationDialog {
  @Input() visible: boolean = false;
  @Input() entityName: string = 'item';
  @Output() confirm = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  onConfirm() {
    this.confirm.emit();
  }

  onCancel() {
    this.cancel.emit();
  }
}


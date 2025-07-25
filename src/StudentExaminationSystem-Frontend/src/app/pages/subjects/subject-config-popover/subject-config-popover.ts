import { Component, Input, Output, EventEmitter } from '@angular/core';
import { PopoverModule } from 'primeng/popover';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-subject-config-popover',
  templateUrl: './subject-config-popover.html',
  styleUrls: ['./subject-config-popover.scss'],
  imports: [PopoverModule, ButtonModule]
})
export class SubjectConfigPopover {
  @Input() hasConfiguration!: Boolean;
  @Input() subjectId!: number;
  @Output() viewConfig = new EventEmitter<number>();
  @Output() editConfig = new EventEmitter<number>();
  @Output() createConfig = new EventEmitter<number>();

  onViewConfig() {
    this.viewConfig.emit(this.subjectId);
  }

  onEditConfig() {
    this.editConfig.emit(this.subjectId);
  }

  onCreateConfig() {
    this.createConfig.emit(this.subjectId);
  }
}

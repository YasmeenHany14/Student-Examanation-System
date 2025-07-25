import { Component, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { PopoverModule } from 'primeng/popover';
import { ButtonModule } from 'primeng/button';
import { Popover } from 'primeng/popover';

@Component({
  selector: 'app-subject-config-popover',
  templateUrl: './subject-config-popover.html',
  styleUrls: ['./subject-config-popover.scss'],
  imports: [PopoverModule, ButtonModule]
})
export class SubjectConfigPopover {
  @ViewChild('op') popover!: Popover;
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

  toggle($event: MouseEvent) {
    this.popover.toggle($event);
  }
}

import { Directive, ElementRef, Input, OnInit, ViewContainerRef, ComponentRef, OnDestroy } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { FormErrorComponent } from '../components/form-error/form-error';
import { inject } from '@angular/core';

@Directive({
  selector: '[appFormGroupError]',
  standalone: true
})
export class FormGroupErrorDirective implements OnInit, OnDestroy {
  @Input('appFormGroupError') formGroup!: FormGroup;
  @Input() groupName?: string;

  private viewContainer = inject(ViewContainerRef);
  private errorComponentRef?: ComponentRef<FormErrorComponent>;

  constructor(
    private el: ElementRef,
  ) {}

  ngOnInit() {
    if (this.formGroup) {
      this.errorComponentRef = this.viewContainer.createComponent(FormErrorComponent);

      this.errorComponentRef.instance.form = this.formGroup;
      if (this.groupName) {
        this.errorComponentRef.instance.fieldName = this.groupName;
      }
    }
  }

  ngOnDestroy() {
    if (this.errorComponentRef) {
      this.errorComponentRef.destroy();
    }
  }
}

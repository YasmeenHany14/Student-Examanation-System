import { Directive, ElementRef, Input, OnInit, ViewContainerRef, ComponentRef, OnDestroy } from '@angular/core';
import { FormGroup, NgControl } from '@angular/forms';
import { FormErrorComponent } from '../components/form-error/form-error';
import { inject } from '@angular/core';

@Directive({
  selector: '[appAutoFormError]',
  standalone: true
})
export class AutoFormErrorDirective implements OnInit, OnDestroy {
  @Input('appAutoFormError') form!: FormGroup;
  private viewContainer = inject(ViewContainerRef);
  private ngControl = inject(NgControl);

  private errorComponentRef?: ComponentRef<FormErrorComponent>;

  constructor(
    private el: ElementRef,
  ) {}

  ngOnInit() {
    if (this.form && this.ngControl?.name) {
      this.errorComponentRef = this.viewContainer.createComponent(FormErrorComponent);

      this.errorComponentRef.instance.form = this.form;
      this.errorComponentRef.instance.fieldName = this.ngControl.name as string;
    }
  }

  ngOnDestroy() {
    if (this.errorComponentRef) {
      this.errorComponentRef.destroy();
    }
  }
}

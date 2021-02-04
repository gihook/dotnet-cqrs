import {
  Component,
  AfterViewInit,
  ComponentFactoryResolver,
  Input,
  ViewChild,
} from '@angular/core';
import { ControlData } from 'src/app/models/control-data';
import { FormGroup } from '@angular/forms';
import { TemplateDirective } from 'src/app/directives/template/template.directive';
import { IFormControl } from '../i-form-control';
import { FormControlsMapperService } from '../../services/form-controls-mapper.service';

@Component({
  selector: 'app-dynamic-field',
  templateUrl: './dynamic-field.component.html',
  styleUrls: ['./dynamic-field.component.scss'],
})
export class DynamicFieldComponent implements AfterViewInit {
  @ViewChild(TemplateDirective, { static: true }) viewChild: TemplateDirective;

  @Input() control: ControlData;
  @Input() formGroup: FormGroup;

  constructor(
    private componentFactoryResolver: ComponentFactoryResolver,
    private formControlsMapper: FormControlsMapperService
  ) {}

  ngAfterViewInit() {
    const type = this.formControlsMapper.getType(this.control.type);
    const componentFactory = this.componentFactoryResolver.resolveComponentFactory<IFormControl>(
      type as any
    );

    const containerRef = this.viewChild.viewContainerRef;
    containerRef.clear();
    const component = containerRef.createComponent(componentFactory);
    component.instance.control = this.control;
    component.instance.formGroup = this.formGroup;
    component.changeDetectorRef.detectChanges();
  }
}

import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ControlData } from 'src/app/models/control-data';
import { IFormControl } from '../i-form-control';

@Component({
  selector: 'app-text-field',
  templateUrl: './text-field.component.html',
  styleUrls: ['./text-field.component.scss'],
})
export class TextFieldComponent implements IFormControl {
  static formControlType = 'text';

  @Input() control: ControlData;
  @Input() formGroup: FormGroup;

  get hasErrors() {
    return this.formGroup.get(this.control.controlName).errors;
  }
}

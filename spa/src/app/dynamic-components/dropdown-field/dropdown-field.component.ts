import { Component, Input } from '@angular/core';
import { ControlData } from 'src/app/models/control-data';
import { FormGroup } from '@angular/forms';
import { IFormControl } from '../i-form-control';

@Component({
  selector: 'app-dropdown-field',
  templateUrl: './dropdown-field.component.html',
  styleUrls: ['./dropdown-field.component.scss'],
})
export class DropdownFieldComponent implements IFormControl {
  static formControlType = 'dropdown';

  @Input() control: ControlData;
  @Input() formGroup: FormGroup;

  get hasErrors() {
    return this.formGroup.get(this.control.controlName).errors;
  }
}

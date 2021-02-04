import { FormGroup } from '@angular/forms';
import { ControlData } from '../models/control-data';

export interface IFormControl {
  control: ControlData;
  formGroup: FormGroup;
}

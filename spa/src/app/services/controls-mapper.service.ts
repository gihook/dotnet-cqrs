import { Injectable } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ControlData } from '../models/control-data';
import { ValidatorsMapperService } from '../services/validators-mapper.service';

@Injectable({ providedIn: 'root' })
export class ControlsMapperService {
  constructor(
    private fb: FormBuilder,
    private validatorsMapper: ValidatorsMapperService
  ) {}

  mapControlsToFormGroup(inputControls: ControlData[]) {
    return inputControls.reduce(
      (formGroup, { controlName, value, validators }) => {
        const controlValidators = (validators || []).map((validatorData) =>
          this.validatorsMapper.getValidator(validatorData)
        );
        formGroup.addControl(
          controlName,
          this.fb.control(value, controlValidators)
        );
        return formGroup;
      },
      new FormGroup({})
    );
  }
}

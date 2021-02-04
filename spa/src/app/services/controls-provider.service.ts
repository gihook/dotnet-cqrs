import { Injectable } from '@angular/core';
import { ControlData } from '../models/control-data';
import { ActionInfo } from '../action-models/action-info';

@Injectable({ providedIn: 'root' })
export class ControlsProviderService {
  getControls(actionInfo: ActionInfo): ControlData[] {
    return actionInfo.parameters.map((parameter) => {
      const controlData: ControlData = {
        input: { label: parameter.name },
        type: 'text',
        controlName: parameter.name,
        validators: [{ type: 'required' }],
      };

      return controlData;
    });
  }
}

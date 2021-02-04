import { ActionParameter } from './action-parameter';

export interface ActionInfo {
  name: string;
  type: string;
  fullName: string;
  parameters: ActionParameter[];
  returnType: string;
}

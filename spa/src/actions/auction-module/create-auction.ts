import { BaseAction } from '../../action-models/base-action';

interface Input {
  name: string;
  descritpion: string;
  initialPrice: number;
}

interface Output {}

export class CreateAuction extends BaseAction<Input, Output> {
  getActionName(): string {
    return 'CreateAuction';
  }
}

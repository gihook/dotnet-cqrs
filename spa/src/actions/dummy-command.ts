import { BaseAction } from '../action-models/base-action';

interface Input {
  id: number;
  name: string;
  scopes: string[];
}

interface ReturnType {
  message: string;
  scopes: string[];
}

export class DummyCommand extends BaseAction<Input, ReturnType> {
  getActionName(): string {
    return 'DummyCommand';
  }
}

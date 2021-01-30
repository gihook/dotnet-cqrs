import { BaseAction } from '../action-models/base-action';

interface Input {
  count: number;
  title: string;
}

export class DummyQuery extends BaseAction<Input, string> {
  getActionName(): string {
    return 'DummyQuery';
  }
}

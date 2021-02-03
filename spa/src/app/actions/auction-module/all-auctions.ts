import { BaseAction } from '../../action-models/base-action';

export class AllAuctions extends BaseAction<void, void> {
  getActionName(): string {
    return 'AllAuctions';
  }
}

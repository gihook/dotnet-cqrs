import { BaseAction } from '../../action-models/base-action';

export class PlaceBid extends BaseAction<
  { id: number; priceValue: number },
  {}
> {
  getActionName(): string {
    return 'PlaceBid';
  }
}

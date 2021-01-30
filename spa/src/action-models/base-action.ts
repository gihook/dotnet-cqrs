export abstract class BaseAction<T, R> {
  // NOTE: dummy field used for typings
  protected returnType: R;

  abstract getActionName(): string;

  constructor(public input: T) {}
}

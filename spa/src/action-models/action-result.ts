enum ResultStatus {
  Ok = 0,
  BadRequest = 1,
  Unauthorized = 2,
}

interface ValidationError {
  fieldName: string;
  value: any;
  errorCode: string;
}

export interface ActionResult<T> {
  resultStatus: ResultStatus;
  resultData: T;
  validationErrors: ValidationError[];
}

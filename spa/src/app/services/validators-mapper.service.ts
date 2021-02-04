import { Validators } from '@angular/forms';
import { Injectable } from '@angular/core';

const VALIDATORS = {
  required: () => Validators.required,
  maxLength: ({ length }) => Validators.maxLength(length),
  minLength: ({ length }) => Validators.minLength(length),
};

@Injectable({ providedIn: 'root' })
export class ValidatorsMapperService {
  getValidator({ type, data }: { type: string; data?: any }) {
    return VALIDATORS[type](data);
  }
}

import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class FormControlsMapperService {
  private types: { [key: string]: any } = {};

  registerType(name: string, type: any) {
    this.types[name] = type;
  }

  getType(typeName: string) {
    return this.types[typeName];
  }
}

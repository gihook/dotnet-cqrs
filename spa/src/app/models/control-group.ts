export interface ControlGroup {
  controls: { [key: string]: Control };
}

export interface Control {
  type: string;
  value: any;
  data: any;
  validators: { type: string; data?: any }[];
}

export interface ControlArray {}

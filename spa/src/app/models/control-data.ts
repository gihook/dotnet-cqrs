export interface ControlData {
  type: string;
  controlName: string;
  input: any;
  value?: string;
  controls?: ControlData[];
  validators?: { type: string; data?: any }[];
}

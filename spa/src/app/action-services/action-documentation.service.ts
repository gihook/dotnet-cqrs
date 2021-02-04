import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActionInfo } from '../action-models/action-info';

@Injectable({ providedIn: 'root' })
export class ActionDocumentationService {
  constructor(private httpClient: HttpClient) {}

  getAllActions() {
    return this.httpClient.get<ActionInfo[]>('/actions');
  }

  actionInfo(actionName: string) {
    return this.httpClient.get<ActionInfo>(`/info/${actionName}`);
  }
}

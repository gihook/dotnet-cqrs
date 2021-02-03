import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class ActionDocumentationService {
  constructor(private httpClient: HttpClient) {}

  getAllActions() {
    return this.httpClient.get('/actions');
  }

  actionInfo(actionName: string) {
    return this.httpClient.get(`/info/${actionName}`);
  }
}

import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { BaseAction } from '../action-models/base-action';
import { Observable } from 'rxjs';
import { ActionResult } from '../action-models/action-result';
import { map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class HttpActionExecutorService {
  constructor(private httpClient: HttpClient) {}

  executeQuery<T, R>(action: BaseAction<T, R>): Observable<R> {
    var params = new HttpParams({ fromObject: action.input as any });
    var result = this.httpClient
      .get<ActionResult<R>>(`/query/${action.getActionName()}`, {
        params,
        headers: this.getHeaders(),
      })
      .pipe(map(({ resultData }) => resultData));

    return result;
  }

  executeCommand<T, R>(action: BaseAction<T, R>): Observable<R> {
    var result = this.httpClient
      .post<ActionResult<R>>(
        `/command/${action.getActionName()}`,
        action.input,
        {
          headers: this.getHeaders(),
        }
      )
      .pipe(map(({ resultData }) => resultData));

    return result;
  }

  executeCommandByName(actionName: string, params: any) {
    return this.httpClient.post(`/command/${actionName}`, params, {
      headers: this.getHeaders(),
    });
  }

  executeQueryByName(actionName: string, params: any) {
    return this.httpClient.post(`/query/${actionName}`, {
      params,
      headers: this.getHeaders(),
    });
  }

  // TODO: needs to pass access token
  private getHeaders() {
    return new HttpHeaders();
  }
}

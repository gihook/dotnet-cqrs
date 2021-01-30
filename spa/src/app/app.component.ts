import { Component } from '@angular/core';
import { HttpActionExecutorService } from '../action-services/http-action-executor.service';
import { DummyQuery } from '../actions/dummy-query';
import { DummyCommand } from '../actions/dummy-command';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  queryResult: Observable<string>;
  commandResult;

  constructor(private actionExecutor: HttpActionExecutorService) {}

  ngOnInit() {
    this.queryResult = this.actionExecutor.executeQuery(
      new DummyQuery({ title: 'Test Title', count: 1000 })
    );

    this.commandResult = this.actionExecutor
      .executeCommand(
        new DummyCommand({
          id: 1234,
          name: 'Nikola',
          scopes: ['Test1', 'Test2'],
        })
      )
      .pipe(tap((data) => console.log({ scopes: data.scopes })));
  }
}

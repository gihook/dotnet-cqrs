import { Component } from '@angular/core';
import { HttpActionExecutorService } from '../action-services/http-action-executor.service';
import {
  AllAuctions,
  CreateAuction,
  PlaceBid,
} from '../actions/auction-module';
import { Observable } from 'rxjs';
import { ActionDocumentationService } from '../action-services/action-documentation.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  queryResult$: Observable<void>;
  commandResult$: Observable<{}>;
  allActions$: Observable<{}>;

  constructor(
    private actionExecutor: HttpActionExecutorService,
    private actionDocumentationService: ActionDocumentationService
  ) {}

  ngOnInit() {
    this.allActions$ = this.actionDocumentationService.getAllActions();
  }

  callPlaceBid() {
    this.commandResult$ = this.actionExecutor.executeCommand(
      new PlaceBid({ id: 1, priceValue: 2000 })
    );
  }

  callCreateAuction() {
    this.commandResult$ = this.actionExecutor.executeCommand(
      new CreateAuction({
        name: 'Angular Sales',
        descritpion: 'Blah',
        initialPrice: 100,
      })
    );
  }

  callAllAuctions() {
    this.queryResult$ = this.actionExecutor.executeQuery(new AllAuctions());
  }

  // TOOD: naive implementation correct it
  executeAction(action, $event: MouseEvent) {
    $event.preventDefault();

    const buttonElement = $event.srcElement as any;
    const formElement = buttonElement.parentElement;
    const formData: any = new FormData(formElement as any);
    const entries = Array.from(formData.entries());
    const params = entries.reduce((acc, [name, value]) => {
      acc[name] = value;
      return acc;
    }, {});

    const actionType: string = action.type.toLowerCase();
    if (actionType.includes('query'))
      this.actionExecutor.executeQueryByName(action.name, params).toPromise();

    if (actionType.includes('command'))
      this.actionExecutor.executeCommandByName(action.name, params).toPromise();
  }
}

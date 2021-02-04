import { Component } from '@angular/core';
import { HttpActionExecutorService } from './action-services/http-action-executor.service';
import { AllAuctions, CreateAuction, PlaceBid } from './actions/auction-module';
import { Observable } from 'rxjs';
import { ActionDocumentationService } from './action-services/action-documentation.service';
import { ControlsProviderService } from './services/controls-provider.service';
import { map } from 'rxjs/operators';
import { FormGroup } from '@angular/forms';
import { ControlsMapperService } from './services/controls-mapper.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  queryResult$: Observable<void>;
  commandResult$: Observable<{}>;
  allActions$: Observable<{}>;
  formGroups: { [key: string]: FormGroup } = {};

  constructor(
    private actionExecutor: HttpActionExecutorService,
    private actionDocumentationService: ActionDocumentationService,
    private controlsProviderService: ControlsProviderService,
    private controlsMapperService: ControlsMapperService
  ) {}

  ngOnInit() {
    this.allActions$ = this.actionDocumentationService.getAllActions().pipe(
      map((actions) => {
        return actions.map((a) => {
          const title = a.name;
          const controls = this.controlsProviderService.getControls(a);
          this.formGroups[
            title
          ] = this.controlsMapperService.mapControlsToFormGroup(controls);

          return { title, controls, actionData: a };
        });
      })
    );
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
  executeAction(
    action: { actionType: string; name: string },
    formData: FormGroup
  ) {
    console.log({ action, formData });

    const actionType: string = action.actionType.toLowerCase();
    if (actionType.includes('query'))
      this.actionExecutor
        .executeQueryByName(action.name, formData.value)
        .toPromise();

    if (actionType.includes('command'))
      this.actionExecutor
        .executeCommandByName(action.name, formData.value)
        .toPromise();
  }
}

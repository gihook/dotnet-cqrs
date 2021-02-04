import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { TemplateDirective } from './directives/template/template.directive';
import { TextFieldComponent } from './dynamic-components/text-field/text-field.component';
import { DropdownFieldComponent } from './dynamic-components/dropdown-field/dropdown-field.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DynamicFieldComponent } from './dynamic-components/dynamic-field/dynamic-field.component';
import { FormControlsMapperService } from './services/form-controls-mapper.service';

const entryComponents = [DropdownFieldComponent, TextFieldComponent];

@NgModule({
  declarations: [
    AppComponent,
    TemplateDirective,
    DynamicFieldComponent,
    ...entryComponents,
  ],
  imports: [HttpClientModule, BrowserModule, FormsModule, ReactiveFormsModule],
  providers: [],
  bootstrap: [AppComponent],
  entryComponents,
})
export class AppModule {
  constructor(private formControlsMapper: FormControlsMapperService) {
    entryComponents.forEach((e) =>
      this.formControlsMapper.registerType(e.formControlType, e)
    );
  }
}

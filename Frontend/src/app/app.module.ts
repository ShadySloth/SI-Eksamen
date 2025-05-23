import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ImagePageComponent } from './pages/image-page/image-page.component';
import {HttpClientModule} from "@angular/common/http";
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { TrainingStatusComponent } from './pages/training-status/training-status.component';
import {FormsModule} from "@angular/forms";
import { DatasetPageComponent } from './pages/dataset-page/dataset-page.component';

@NgModule({
  declarations: [
    AppComponent,
    ImagePageComponent,
    SidebarComponent,
    TrainingStatusComponent,
    DatasetPageComponent
  ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        HttpClientModule,
        FormsModule
    ],
  providers: [  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

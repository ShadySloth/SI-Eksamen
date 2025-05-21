import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {ImagePageComponent} from "./pages/image-page/image-page.component";
import {TrainingStatusComponent} from "./pages/training-status/training-status.component";
import { DatasetPageComponent } from './pages/dataset-page/dataset-page.component';

const routes: Routes = [
  {path : '', redirectTo: '/image', pathMatch: 'full'},
  {path : 'datasets', component: DatasetPageComponent},
  {path : "image", component: ImagePageComponent},
  {path: "training", component: TrainingStatusComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {ImagePageComponent} from "./pages/image-page/image-page.component";

const routes: Routes = [
  {path : '', redirectTo: '/image', pathMatch: 'full'},
  {path : "image", component: ImagePageComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

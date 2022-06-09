import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { HomeDocsComponent } from "./home-docs/home-docs.component";

const routes: Routes = [
  {
    path: "",
    component: HomeDocsComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DsHomeRoutingModule {}

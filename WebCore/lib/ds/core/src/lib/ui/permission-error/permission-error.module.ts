import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { PermissionErrorComponent } from "./permission-error.component";
import { MaterialModule } from "../material/material.module";

@NgModule({
  imports: [
    MaterialModule,
    CommonModule,
  ],
  declarations: [
    PermissionErrorComponent
  ],
  exports: [
    PermissionErrorComponent
  ]
})
export class PermissionErrorModule {}
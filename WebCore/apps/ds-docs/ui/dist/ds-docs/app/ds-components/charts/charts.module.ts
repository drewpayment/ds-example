import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MarkdownModule } from 'ngx-markdown';
import { ChartDocsComponent } from './chart-docs/chart-docs.component';
import { MaterialModule } from '@ds/core/ui/material';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { ChartLegendComponent } from './chart-legend/chart-legend.component';
import { ChartLegendLevelsComponent } from './chart-legend-levels/chart-legend-levels.component';
import { ChartExampleComponent } from './chart-example/chart-example.component';
import { ChartColorsComponent } from './chart-colors/chart-colors.component';
import { BajaBlastComponent } from './chart-colors/baja-blast/baja-blast.component';
import { GuacamoleComponent } from './chart-colors/guacamole/guacamole.component';
import { NachoCheeseComponent } from './chart-colors/nacho-cheese/nacho-cheese.component';
import { YoQuieroTacoBellComponent } from './chart-colors/yo-quiero-taco-bell/yo-quiero-taco-bell.component';
import { LegendCenterComponent } from './legend-center/legend-center.component';

@NgModule({
  declarations: [ChartDocsComponent, ChartLegendComponent, ChartLegendLevelsComponent, ChartExampleComponent, ChartColorsComponent, BajaBlastComponent, GuacamoleComponent, NachoCheeseComponent, YoQuieroTacoBellComponent, LegendCenterComponent],
  imports: [
    CommonModule,
    MaterialModule,
    DsCardModule,
    MarkdownModule.forChild()
  ]
})
export class ChartsModule { }

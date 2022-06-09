import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfigureCostCentersDialogComponent } from './configure-cost-centers-dialog.component';

describe('ConfigureCostCentersDialogComponent', () => {
  let component: ConfigureCostCentersDialogComponent;
  let fixture: ComponentFixture<ConfigureCostCentersDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ConfigureCostCentersDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfigureCostCentersDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

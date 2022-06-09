import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CertifyI9DialogComponent } from './certify-I9-dialog.component';

describe('AddResourceDialogComponent', () => {
  let component: CertifyI9DialogComponent;
  let fixture: ComponentFixture<CertifyI9DialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CertifyI9DialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CertifyI9DialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

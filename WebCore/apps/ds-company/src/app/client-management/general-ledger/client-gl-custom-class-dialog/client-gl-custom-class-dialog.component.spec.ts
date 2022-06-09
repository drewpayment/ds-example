import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientGlCustomClassDialogComponent } from './client-gl-custom-class-dialog.component';

describe('ClientGlCustomClassDialogComponent', () => {
  let component: ClientGlCustomClassDialogComponent;
  let fixture: ComponentFixture<ClientGlCustomClassDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientGlCustomClassDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientGlCustomClassDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

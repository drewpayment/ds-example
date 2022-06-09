import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BajaBlastComponent } from './baja-blast.component';

describe('BajaBlastComponent', () => {
  let component: BajaBlastComponent;
  let fixture: ComponentFixture<BajaBlastComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BajaBlastComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BajaBlastComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

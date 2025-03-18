import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageBookingsComponent } from './managebookings.component';

describe('ManagebookingsComponent', () => {
  let component: ManageBookingsComponent;
  let fixture: ComponentFixture<ManageBookingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageBookingsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageBookingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

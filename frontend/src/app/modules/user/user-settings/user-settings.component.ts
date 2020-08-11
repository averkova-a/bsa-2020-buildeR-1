import { Component, OnInit, Input } from '@angular/core';
import { User } from '@shared/models/user';
import { UserSettingsService } from '@core/services/user-settings.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
@Component({
  selector: 'app-user-settings',
  templateUrl: './user-settings.component.html',
  styleUrls: ['./user-settings.component.sass']
})
export class UserSettingsComponent implements OnInit {

  @Input() details: User = {} as User;
  public settingsForm: FormGroup;
  constructor(settingsService: UserSettingsService) { }

  ngOnInit(): void {
    this.settingsForm = new FormGroup({
      firstName: new FormControl(this.details.firstName,
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(30),
          Validators.pattern("^(?![-'])(?!.*--)(?!.*'')[[A-Za-z-']+(?<![-'])$")
        ]),
      lastName: new FormControl(this.details.lastName,
         [
           Validators.required,
           Validators.minLength(2),
           Validators.maxLength(30),
           Validators.pattern("^(?![-'])(?!.*--)(?!.*'')[[A-Za-z-']+(?<![-'])$")
        ]),
      email: new FormControl(this.details.email,
        [
           Validators.email,
           Validators.required
        ]),
        location: new FormControl(this.details.location,
          [
            Validators.minLength(2),
            Validators.maxLength(30),
            Validators.pattern("^(?![-'])(?!.*--)(?!.*'')[[A-Za-z-']+(?<![-'])$")
        ]),
        username: new FormControl(this.details.username),
        bio : new FormControl(this.details.bio,
          [
            Validators.maxLength(200)
          ])
    });
  }

}

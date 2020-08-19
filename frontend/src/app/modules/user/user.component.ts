import { Component, OnInit } from '@angular/core';
import {User} from "../../shared/models/user/user";
import {ActivatedRoute} from "@angular/router";
import {ModalCropperService} from "../../core/services/modal-cropper.service";
import {UserService} from "../../core/services/user.service";

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.sass']
})
export class UserComponent implements OnInit {
  tab = 1;

  currentUser: User = {} as User;

  constructor(private route: ActivatedRoute, private cropper: ModalCropperService, private userService: UserService) { }

  ngOnInit(): void {
    this.route.data.subscribe( data => this.currentUser = data.user);
    this.userService.userLogoUrl.subscribe( url => {
      this.currentUser.avatarUrl = url;
    });
    this.userService.userLogoUserName.subscribe( userName => {
      this.currentUser.username = userName;
    });
  }

  change(id: number) {
    this.tab = id;
  }

  async open(){
    const file = await this.cropper.open();
    if (file){
      console.log('we have cropped ' + typeof(file));
      // now we can use it for saving image logic
    }
    else{
      console.log('Image didn`t change');
    }
  }

}

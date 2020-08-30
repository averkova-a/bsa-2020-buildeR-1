import { Component, OnInit } from '@angular/core';
import { GroupService } from '../../../core/services/group.service';
import { ActivatedRoute } from '@angular/router';
import { TeamMember } from '../../../shared/models/group/team-member';
import { UserRole } from '../../../shared/models/group/user-role';
import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map } from 'rxjs/operators';
import { User } from '../../../shared/models/user/user';
import { UserService } from '../../../core/services/user.service';
import { TeamMemberService } from '../../../core/services/team-member.service';
import { ToastrNotificationsService } from '@core/services/toastr-notifications.service';
import { FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-group-members',
  templateUrl: './group-members.component.html',
  styleUrls: ['./group-members.component.sass']
})
export class GroupMembersComponent implements OnInit {
  groupId: number;
  model: any;
  members: TeamMember[];
  newTeamMember: TeamMember;
  users: User[];
  userRole: typeof UserRole = UserRole;
  roles = [
    UserRole.Admin,
    UserRole.Creator,
    UserRole.Viewer,
    UserRole.User
  ];
  memberForm: FormGroup;
  constructor(
    private groupService: GroupService,
    private route: ActivatedRoute,
    private userService: UserService,
    private teamMemberService: TeamMemberService,
    private toastrService: ToastrNotificationsService,
  ) {
    route.parent.params.subscribe(
      (params) => this.groupId = params.groupId);
  }
  ngOnInit(): void {
    this.getGroupMembers(this.groupId);
    this.getUsers();
    this.memberForm = new FormGroup({
      user: new FormControl(),
      dropdown: new FormControl(this.userRole[UserRole.User])
    });
  }

  getGroupMembers(groupId: number) {
    this.groupService.getMembersByGroup(groupId).subscribe(res => this.members = res.body);
  }
  getUsers() {
    this.userService.getUsers().subscribe(res => this.users = res.body);
  }
  changeMemberRole(member: TeamMember, newUserRole: UserRole) {
    member.memberRole = newUserRole;
    this.teamMemberService.updateMember(member).subscribe(() =>
      this.toastrService.showSuccess('Member role successfully changed'),
      (err) => {
        this.toastrService.showError(err);
      }
    );
  }
  onSubmit() {
    this.newTeamMember = this.memberForm.value as TeamMember;
    this.newTeamMember.groupId = this.groupId;
    this.newTeamMember.userId = this.memberForm.controls.user.value.id;
    this.newTeamMember.memberRole = this.memberForm.controls.dropdown.value;
    this.teamMemberService.createMember(this.newTeamMember).subscribe(() => this.getGroupMembers(this.groupId));
  }
  search = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      map(term => term.length < 2 ? []
        : this.users.filter(v => v.username.toLowerCase().indexOf(term.toLowerCase()) > -1 ||
          v.email.toLowerCase().indexOf(term.toLowerCase()) > -1).slice(0, 10))
    )
  formatter = (x: { username: string }) => x.username;
  inputFormatter = (x: { username: string }) => x.username;
}

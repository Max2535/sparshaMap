/*import React from 'react';
import { Col, Grid, Row } from 'react-bootstrap';
import NavMenu from './NavMenu';

export default props => (
  <Grid fluid>
    <Row>
      <Col sm={3}>
        <NavMenu />
      </Col>
      <Col sm={9}>
        {props.children}
      </Col>
    </Row>
  </Grid>
);*/
import React from 'react';
import {
    Table,
    Container,
    Row,
    Col,
    Collapse,
    Navbar,
    NavbarToggler,
    NavbarBrand,
    Nav,
    NavItem,
    NavLink,
    UncontrolledDropdown,
    DropdownToggle,
    DropdownMenu,
    DropdownItem,
    Breadcrumb,
    BreadcrumbItem,
    Badge
} from 'reactstrap';
import { Glyphicon } from 'react-bootstrap';
export default class Home extends React.Component {
    constructor(props) {
        super(props);

        this.toggle = this.toggle.bind(this);
        this.state = {
            isOpen: false
        };
    }
    toggle() {
        this.setState({
            isOpen: !this.state.isOpen
        });
    }
    render() {
        var check = false;
        return (
            <div>{(window.location.href).indexOf('Member') < 0 ?
                <div>
                <Navbar color="dark" dark expand="sm">
                    <NavbarBrand href="/">reactstrap</NavbarBrand>
                    <NavbarToggler onClick={this.toggle} />
                    <Collapse isOpen={this.state.isOpen} navbar>
                        <Nav className="ml-auto" navbar>
                            <NavItem>
                                {check = (window.location.href).indexOf('User') > 0}
                                <NavLink active={check} href="/User"><h5>{check ? <Badge color="secondary">จัดการสมาชิก</Badge> : "จัดการสมาชิก"}</h5></NavLink>
                            </NavItem>
                            <NavItem>
                                {check = (window.location.href).indexOf('Ads') > 0}
                                <NavLink active={check} href="/Ads"><h5>{check ? <Badge color="secondary">จัดการโฆษณา</Badge> : "จัดการโฆษณา"}</h5></NavLink>
                            </NavItem>
                            <NavItem>
                                {check = (window.location.href).indexOf('Promotion') > 0}
                                <NavLink active={check} href="/Promotion"><h5>{check ? <Badge color="secondary">จัดการโปรโมชั่น</Badge> : "จัดการโปรโมชั่น"}</h5></NavLink>
                            </NavItem>
                            <NavItem>
                                {check = (window.location.href).indexOf('Notification') > 0}
                                <NavLink active={check} href="/Notification"><h5>{check ? <Badge color="secondary">การแจ้งเตือน</Badge> : "การแจ้งเตือน"}</h5></NavLink>
                            </NavItem>
                            <NavItem>
                                {check = (window.location.href).indexOf('Mapadmin') > 0}
                                <NavLink active={check} href="/Mapadmin"><h5>{check ? <Badge color="secondary">Map</Badge> : "Map"}</h5></NavLink>
                            </NavItem>
                            <UncontrolledDropdown nav inNavbar>
                                <DropdownToggle nav caret>
                                    User Login
                                    </DropdownToggle>
                                <DropdownMenu right>
                                    <DropdownItem>
                                        Option 1
                                        </DropdownItem>
                                    <DropdownItem>
                                        Option 2
                                        </DropdownItem>
                                    <DropdownItem divider />
                                    <DropdownItem>
                                        Reset
                                        </DropdownItem>
                                </DropdownMenu>
                            </UncontrolledDropdown>
                        </Nav>
                    </Collapse>
                </Navbar>
                <Container>
                    {this.props.children}
                    </Container>
                </div> : ""}
            </div>
        );
    }
}
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
    Container,
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
    Badge
} from 'reactstrap';
import { Glyphicon } from 'react-bootstrap';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { actionCreators } from '../store/session';


const jwt = require('jsonwebtoken');
class Layout extends React.Component {
    constructor(props) {
        super(props);

        this.toggle = this.toggle.bind(this);
        this.state = {
            isOpen: false,
            name:""
        };
    }
    componentDidMount() {
        //ต้อง call จาก api
        try {
            var decoded = jwt.verify(this.props.token, 'Super secret key');//debug
            this.setState({
                name: decoded.unique_name
            });
        } catch (error) {
            console.log(error);
            if (window.location.href.indexOf("Login") < 0) {

                var userData = sessionStorage.getItem('userData');
                if (userData===null) {
                    window.location.href = "/Login";//goto login
                }
            }
        }
        
    }

    toggle() {
        this.setState({
            isOpen: !this.state.isOpen
        });
    }
    render() {
        var check = false;
        return (
            <div>{(window.location.href).indexOf('Member') < 0?
                <div>
                <Navbar color="dark" dark expand="sm">
                    <NavbarBrand href="/">SparSha</NavbarBrand>
                    <NavbarToggler onClick={this.toggle} />
                    <Collapse isOpen={this.state.isOpen} navbar>
                            <Nav className="ml-auto" navbar>
                                <NavItem>
                                    <Glyphicon glyph="align-left" />
                                    <Link a className='btn btn-default pull-left' to={`/Users`}>จัดการสมาชิก</Link>
                            </NavItem>
                            <NavItem>
                                    <Link a className='btn btn-default pull-left' to={`/Ads`}>จัดการโฆษณา</Link>
                            </NavItem>
                            <NavItem>
                                    <Link a className='btn btn-default pull-left' to={`/Promotion`}>จัดการโปรโมชั่น</Link>
                            </NavItem>
                            <NavItem>
                                    <Link a className='btn btn-default pull-left' to={`/Notification`}>การแจ้งเตือน</Link>
                            </NavItem>
                            <NavItem>
                                    <Link a className='btn btn-default pull-left' to={`/Mapadmin`}>Map</Link>
                            </NavItem>
                            <UncontrolledDropdown nav inNavbar>
                                    <DropdownToggle nav caret>
                                        {this.state.name}
                                    </DropdownToggle>
                                <DropdownMenu right>
                                    <DropdownItem>
                                        แจ้งเตือน
                                        </DropdownItem>
                                    <DropdownItem>
                                        ตั้งค่า
                                        </DropdownItem>
                                    <DropdownItem divider />
                                    <DropdownItem>
                                        ออกจากระบบ
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

export default connect(
    state => state.session,
    dispatch => bindActionCreators(actionCreators, dispatch)
)(Layout);
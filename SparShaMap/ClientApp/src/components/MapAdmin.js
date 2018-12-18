import React, { Component } from "react";
import { Map, TileLayer, Marker, Popup } from "react-leaflet";
import L from "leaflet";
import {
    Card,
    CardTitle,
    Button,
    Modal,
    ModalHeader,
    ModalBody,
    ModalFooter,
    Progress,
    InputGroupAddon,
    Input,
    InputGroup
} from "reactstrap";
import Autocomplete from "react-autocomplete";
import $ from "jquery";
import Select from "react-select";

const myIcon = L.icon({
    iconUrl:
        "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABkAAAApCAYAAADAk4LOAAAFgUlEQVR4Aa1XA5BjWRTN2oW17d3YaZtr2962HUzbDNpjszW24mRt28p47v7zq/bXZtrp/lWnXr337j3nPCe85NcypgSFdugCpW5YoDAMRaIMqRi6aKq5E3YqDQO3qAwjVWrD8Ncq/RBpykd8oZUb/kaJutow8r1aP9II0WmLKLIsJyv1w/kqw9Ch2MYdB++12Onxee/QMwvf4/Dk/Lfp/i4nxTXtOoQ4pW5Aj7wpici1A9erdAN2OH64x8OSP9j3Ft3b7aWkTg/Fm91siTra0f9on5sQr9INejH6CUUUpavjFNq1B+Oadhxmnfa8RfEmN8VNAsQhPqF55xHkMzz3jSmChWU6f7/XZKNH+9+hBLOHYozuKQPxyMPUKkrX/K0uWnfFaJGS1QPRtZsOPtr3NsW0uyh6NNCOkU3Yz+bXbT3I8G3xE5EXLXtCXbbqwCO9zPQYPRTZ5vIDXD7U+w7rFDEoUUf7ibHIR4y6bLVPXrz8JVZEql13trxwue/uDivd3fkWRbS6/IA2bID4uk0UpF1N8qLlbBlXs4Ee7HLTfV1j54APvODnSfOWBqtKVvjgLKzF5YdEk5ewRkGlK0i33Eofffc7HT56jD7/6U+qH3Cx7SBLNntH5YIPvODnyfIXZYRVDPqgHtLs5ABHD3YzLuespb7t79FY34DjMwrVrcTuwlT55YMPvOBnRrJ4VXTdNnYug5ucHLBjEpt30701A3Ts+HEa73u6dT3FNWwflY86eMHPk+Yu+i6pzUpRrW7SNDg5JHR4KapmM5Wv2E8Tfcb1HoqqHMHU+uWDD7zg54mz5/2BSnizi9T1Dg4QQXLToGNCkb6tb1NU+QAlGr1++eADrzhn/u8Q2YZhQVlZ5+CAOtqfbhmaUCS1ezNFVm2imDbPmPng5wmz+gwh+oHDce0eUtQ6OGDIyR0uUhUsoO3vfDmmgOezH0mZN59x7MBi++WDL1g/eEiU3avlidO671bkLfwbw5XV2P8Pzo0ydy4t2/0eu33xYSOMOD8hTf4CrBtGMSoXfPLchX+J0ruSePw3LZeK0juPJbYzrhkH0io7B3k164hiGvawhOKMLkrQLyVpZg8rHFW7E2uHOL888IBPlNZ1FPzstSJM694fWr6RwpvcJK60+0HCILTBzZLFNdtAzJaohze60T8qBzyh5ZuOg5e7uwQppofEmf2++DYvmySqGBuKaicF1blQjhuHdvCIMvp8whTTfZzI7RldpwtSzL+F1+wkdZ2TBOW2gIF88PBTzD/gpeREAMEbxnJcaJHNHrpzji0gQCS6hdkEeYt9DF/2qPcEC8RM28Hwmr3sdNyht00byAut2k3gufWNtgtOEOFGUwcXWNDbdNbpgBGxEvKkOQsxivJx33iow0Vw5S6SVTrpVq11ysA2Rp7gTfPfktc6zhtXBBC+adRLshf6sG2RfHPZ5EAc4sVZ83yCN00Fk/4kggu40ZTvIEm5g24qtU4KjBrx/BTTH8ifVASAG7gKrnWxJDcU7x8X6Ecczhm3o6YicvsLXWfh3Ch1W0k8x0nXF+0fFxgt4phz8QvypiwCCFKMqXCnqXExjq10beH+UUA7+nG6mdG/Pu0f3LgFcGrl2s0kNNjpmoJ9o4B29CMO8dMT4Q5ox8uitF6fqsrJOr8qnwNbRzv6hSnG5wP+64C7h9lp30hKNtKdWjtdkbuPA15nJ7Tz3zR/ibgARbhb4AlhavcBebmTHcFl2fvYEnW0ox9xMxKBS8btJ+KiEbq9zA4RthQXDhPa0T9TEe69gWupwc6uBUphquXgf+/FrIjweHQS4/pduMe5ERUMHUd9xv8ZR98CxkS4F2n3EUrUZ10EYNw7BWm9x1GiPssi3GgiGRDKWRYZfXlON+dfNbM+GgIwYdwAAAAASUVORK5CYII=",
    iconSize: [25, 41],
    iconAnchor: [22, 44],
    popupAnchor: [-3, -36],
    shadowSize: [68, 45],
    shadowAnchor: [22, 44]
});

const brand = L.icon({
    iconUrl:
        "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAABGdBTUEAALGPC/xhBQAAACBjSFJNAAB6JgAAgIQAAPoAAACA6AAAdTAAAOpgAAA6mAAAF3CculE8AAAABmJLR0QA/wD/AP+gvaeTAAAAB3RJTUUH4AsOCBMa3wEeUAAABvRJREFUWMPtVm1sVFkZfs69d2bu3Jk70+nMdDpDaSm0hZa2IARW2MjSZLfgCiq7Jm4wrmBcYyQx6iYG1x9udo0frJuoKFkhqwkkrmtEBBWJVgV2+W7ZFkpbWvrBdKbTmWnne+6de+/cc/wx1GyymCwfMTHh+XfPSe7znPd53vcc4BH+nzGaTGNiPmt/s69/4+nJ23VLfnoAjLH/DTljDOj6NP54Y2Tjl353/NKBC70vMMbIvQrg7pscwMBvD7Yd7hvY+/bA4Pq/jd76zLxaCl6LJe6pCvcsYOHn12KJttf+8c53e8bGt+RpGYOx2fUnh0efWBUKoEzpwxfAGAOtkJN/jk+tfrXn9I9ODI/syJmmFTyPWF6pOj8VfowxZp1TlQ8tQLiX0xPAfmLoZvcvzl/e++7t6fUKYxzhODAQKNREOJ39aDibD8zkctMPVcDo3BxMUOeBc1d2Hrk68GJ/PNliACCEoGIIAwMQzuW8A7F4dST7kATc8ZvMqaXAS3/p+cqx6zf2jGdzPkr4yj5hACEAAUAIlJJeNZJI+COZ7INbsBC2jFpq+GHPmW+9NXD9C7GiIjV6vcllHndENUw+nit4p/L5kAFGAAaTUsdsvuCeKxQfTABjDOFsDvVul/yDf7275zcDg7tjakkUBd7cvrypzzRN+7qGxX9QdEPcd+b8y5PZnB0EYGBcuUz5svkQuqDe7eJ+efnqJ48ODu+KqSURhAPHiCkJgjqRTIVMSqvOjE1sShaLdnAAGEAIMUWLYNgsHz7bdxVwcTqKSD4fOjk8unsqm/WBEAAABbMWdMO+qWnpkZAsj3AcKVl5XgGr7As8n6tzu1K1snz/FjDGQJ7cgcOvvbJ6KJHsNFHJGQNgUIapdNby+rbuN6yCkLwUmRkeS2daUrHZDgCQRVt+ecA/7xDFB6sA6zlGwunsR7KK5l04/Z2QIa2qtplCkWOMcVemo2vi+WI9QMAxhnrZFe8M1qZXBQP3X4EyZRA4ws0rilQyy1yFmgGUYbEspzcvW3K6we1SjvT1P/321WsvzhSKbhBA4nnUedwXax32eK3DbinqhntsLuOI5jNiztAtFCBOwVr2SVY96HJr1Xa7Zpqm+gEBhDEAZGHsApV4wy3w5WfbW4+82t21H4B+djK8rTcS7ShzHEAZQm5X6qmWpfGDl/ueuxqNrUkVlNWzhWJVXtdFnVILwIhAuLIk8JosWPRmvze/qaX54AcE8DwHANRhtWkCxzFQk4AxLKnyxJ5uazn+9T+dSuzZsK45ks2u1VAJCGGAZBe5g5f6nh+ajS9NKqqklcuEACC8gDJjYIxWBpbJsNjhyLUGfG8tcjlvcpOZtD+llfxKueyjjHkACKTrU8wlWmZEgTcABjAGp2hLeOyOyZ9s3yr/+kr/F3sjsZWUIwAYwBFcjyerTo1OdMQV1RaQnanHGxvC29pWJFcvCiqiRQAEASAEDZ6q+V0b1u37/vate8dz+feEQxf69l+6HanhLQL12KyGjeeHX37pGwMra/zUI9mTMUVdRADYON6olSXsO33us7+/PvTCnKbbwXMAYyCmiWrRVmyvXxRuDwbijFLrcGLeNxhP+GJFRdQoBUwTdZKjsHNV+/5Xnnri5xPzmeyuzjYIAafzgqppX+6fCrfqBMTnkLrrYnHl3NjEjEapDA4gFOAIjOFEsvHY4MjXbmWzXnAEKFNUWy3q+iWLxtfUBacjmayzZ+TWitu5nFel1MIIqzQaowiKovZMR+uhbz+5+WcXwjPZDfWhiuV//dWh3sXVnoGyyfwzuVy9Tqm1xe/TPZJoRLM5MaPpNgvHYV1d6EqtLE//eWT0c+mSJloBuqrGH36ms/U9k1GcHBntPD8dbYlrJdkgHE8IAcidgEqS8mx765vf7Hr8e1cj0VTX0gaQO+0tADC7m5eea6/xTy/v9e48fmNk98Wp6caQy+n+WGND1DETMyZSGa9LksYLhuHWTWr32kS9a1nDhEu05U8M3eyczGRqDBACnq9kgjIwxmDjCJq91dM7OlYc+urGxw6MJVPzH29p+g/5nR6rTL85RYFPkpynbt5ae/T60OcvTIa3qJrua/BXK6PJlPsTK5f/GIBx9NqN77QF/Pm8WjKGk/NVGqV85S7gwBPAwhPIFlu51umY7aj1n9uyvPnw82tXnY0VCoXQXUY0ef/HwhVcNIyqv49NrDt9a3zreCqzOVVUg90tTa+H05mVZyemniNAWTNNTuA5wyYIhigIuiyKmmyzRH0OaaTe4xlfGah5Z1Pj4uGgLKcr3UpwN9x19X2vWmtvNNYwFJ8LNnk9U/2xeChRyNdSBsYLPHMJVr3KLpa8TkmplR2qTbDMNVV7Ug6LoFe8+O/Ej/AIC/g3Ri9zYt0ByIgAAAAldEVYdGRhdGU6Y3JlYXRlADIwMTYtMTEtMTRUMDg6MTk6MjYrMDE6MDAXS0vQAAAAJXRFWHRkYXRlOm1vZGlmeQAyMDE2LTExLTE0VDA4OjE5OjI2KzAxOjAwZhbzbAAAABl0RVh0U29mdHdhcmUAQWRvYmUgSW1hZ2VSZWFkeXHJZTwAAABXelRYdFJhdyBwcm9maWxlIHR5cGUgaXB0YwAAeJzj8gwIcVYoKMpPy8xJ5VIAAyMLLmMLEyMTS5MUAxMgRIA0w2QDI7NUIMvY1MjEzMQcxAfLgEigSi4A6hcRdPJCNZUAAAAASUVORK5CYII=",
    iconSize: [25, 41],
    iconAnchor: [22, 44],
    popupAnchor: [-3, -36],
    shadowSize: [68, 45],
    shadowAnchor: [22, 44]
});

let cur_lat = 0;
let cur_lng = 0;

//debug to state
let markers = [
    //lat:13.7481758,lng:100.6182162
    {
        position: [13.7481758, 100.6182162],
        content: "สาขาที่ 1"
    },
    {
        position: [13.7484103, 100.6207369],
        content: "สาขาที่ 2"
    },
    {
        position: [13.7502752, 100.6219909],
        content: "สาขาที่ 3"
    }
];

class MyMarker extends React.Component {
    render() {
        const position = [this.props.lat, this.props.lng];
        console.log(position);
        return (
            <Marker icon={brand} position={position}>
                <Popup>
                    {/* <a
                        href={`https://www.google.com/maps/dir/${cur_lat},${cur_lng}/
              ${this.props.lat},${this.props.lng}`}
                        ref={el => {
                            if (el) {
                                this.anchorElement = el;
                            }
                        }}
                    >
                        <b>{this.props.address}</b>
                    </a>*/}
                    <a><b>{this.props.address}</b></a>
                </Popup>
            </Marker>
        );
    }
}

class MapAdmin extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            location: {
                lat: 0,
                lng: 0
            },
            style: "block",
            cur: true,
            address: "",
            haveUsersLocation: false,
            modal: false,
            zoom: 3,
            value: "",
            branch: "",
            error: false,
            arr: [{ display_name: "", lat: 0, lon: 0 }],
            branchList: [],
            height: 500
        };
        this.handleClickGeo = this.handleClickGeo.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.handleClick = this.handleClick.bind(this);
        this.getLocation = this.getLocation.bind(this);
        this.handleChangeBranch = this.handleChangeBranch.bind(this);
        this.toggle = this.toggle.bind(this);
        this.Search = this.Search.bind(this);
        this.getGeoLocation = this.getGeoLocation.bind(this);
    }

    //ค้นหาตาม lat,lng
    async Search(query) {
        this.setState({
            style: "block"
        });
        const response = await fetch(
            `https://nominatim.openstreetmap.org/search?q=${query}&format=json`
        );
        const json = await response.json();
        this.setState({
            location: {
                lat: json[0].lat,
                lng: json[0].lon
            },
            style: null,
            address: json[0].display_name,
            cur: false,
            haveUsersLocation: true,
            zoom: 15
        });
    }
    componentWillMount() {
        this.setState({ height: window.innerHeight});
    }
    handleChangeBranch(value) {
        this.setState({
            branch: value.value
        });
    }
    //กรณีปิด popup
    toggle() {
        if (this.state.branch === "") {
            this.setState({
                error: !this.state.error
            });
        } else {
            var obj = {
                lat: this.state.location.lat,
                lng: this.state.location.lng,
                address: this.state.address,
                branch: this.state.branch
            };
            console.log(obj);
            (async () => {
                const rawResponse = await fetch("https://httpbin.org/post", {
                    method: "POST",
                    headers: {
                        Accept: "application/json",
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(obj)
                });
                const content = await rawResponse.json();
                this.setState({
                    address: 0,
                    branch: ""
                });
                console.log(content);
            })();
            this.setState({
                modal: !this.state.modal
            });
            alert("บันทึกข้อมูลสำเร็จ");
        }
    }
    //กรณีคลิกตามแผนที่
    getLocation(e) {
        var loc = e.latlng;
        this.Search(loc.lat + "," + loc.lng);
        this.setState({
            modal: !this.state.modal
        });
    }
    //ดึกข้อมูลตำแหน่งปัจจุบัน
    componentDidMount() {
        this.getGeoLocation();
        fetch("https://56d9dd18.ngrok.io/api/sparshaapi/getbranch", {
            method: "POST",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            }
        })
            .then(res => res.json())
            .then(branch => {
                this.setState({ branchList: branch });
                console.log(branch);
            });
    }
    getGeoLocation() {
        navigator.geolocation.getCurrentPosition(
            position => {
                console.log("get location from navigator");
                cur_lat = position.coords.latitude;
                cur_lng = position.coords.longitude;
                this.setState({
                    location: {
                        lat: position.coords.latitude,
                        lng: position.coords.longitude
                    },
                    haveUsersLocation: true,
                    zoom: 15
                });
            },
            () => {
                console.log("get location from ip address");
                fetch("https://ipapi.co/json")
                    .then(res => res.json())
                    .then(location => {
                        cur_lat = location.latitude;
                        cur_lng = location.longitude;
                        this.setState({
                            location: {
                                cur_lat: location.latitude,
                                cur_lng: location.longitude
                            },
                            haveUsersLocation: true,
                            zoom: 15
                        });
                    });
            }
        );
    }
    //แสดงผลลัพท์ค้นหาข้อมูลสถานที่ทั่วไป
    handleClick(value) {
        var data = this.state.arr.filter(e => e.display_name === value);
        this.setState({
            cur: false,
            address: data[0].display_name,
            location: {
                lat: data[0].lat,
                lng: data[0].lon
            },
            haveUsersLocation: true,
            zoom: 15
        });
    }
    handleClickGeo() {
        console.log(this.state.lat + "++" + cur_lat);
        if (this.state.location.lat === cur_lat) {
            window.location.reload();
        }
        this.setState({
            location: {
                lat: cur_lat,
                lng: cur_lng
            },
            zoom: 15
        });
    }
    //กรณีที่ค้นหาข้อมูลสถานที่ทั่งไป
    handleChange(event) {
        this.setState({ value: event.target.value });
        this.setState({
            arr: []
        });
        var geo;
        var arr = [];
        let url =
            "https://nominatim.openstreetmap.org/search?format=json&q=" +
            event.target.value;
        fetch(url)
            .then(res => res.json())
            .then(
                result => {
                    geo = result;
                },
                error => {
                    console.log("error");
                }
            );
        setTimeout(() => {
            try {
                for (var i = 0; i < geo.length; i++) {
                    arr.push({
                        display_name: geo[i].display_name,
                        lat: geo[i].lat,
                        lon: geo[i].lon
                    });
                }
                this.setState({
                    arr: arr
                });
            } catch (error) {
                console.log(error);
            }
        }, 3000);
    }

    render() {
        const position = [this.state.location.lat, this.state.location.lng];
        const positionCur = [cur_lat, cur_lng];
        return (
            <div className="map" style={{ height: this.state.height }}>
                <Map
                    className="map"
                    onClick={e => this.getLocation(e)}
                    center={position}
                    zoom={this.state.zoom}
                >
                    <TileLayer
                        attribution='&amp;copy <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
                        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                    />
                    {this.state.haveUsersLocation ? (
                        <div>
                            <Marker icon={myIcon} position={positionCur}>
                                <Popup>
                                    <h5>ตำแหน่งปัจจุบัน</h5>
                                </Popup>
                            </Marker>
                            <Marker icon={brand} position={position}>
                                <Popup>
                                    <h5>ตำแหน่งสาขาที่เลือก</h5>
                                </Popup>
                            </Marker>
                        </div>
                    ) : (
                            ""
                        )}
                    {markers.map(k => (
                        <MyMarker
                            lat={k.position[0]}
                            lng={k.position[1]}
                            address={k.content}
                        />
                    ))}
                </Map>
                <Card body className="message-form text-center">
                    <CardTitle>{'ค้นหาสถานที่'}</CardTitle>
                    <br />
                    <Autocomplete
                        className="search"
                        getItemValue={item => item.display_name}
                        items={this.state.arr}
                        renderItem={(item, isHighlighted) => (
                            <div
                                style={{ background: isHighlighted ? "lightgray" : "white" }}
                            >
                                {item.display_name}
                            </div>
                        )}
                        value={this.state.value}
                        onChange={this.handleChange}
                        onSelect={value => this.handleClick(value)}
                    />
                </Card>
                <Card body className="loc">
                    <img
                        onClick={this.handleClickGeo}
                        src={require("./asset/geo.png")}
                        style={{ width: 25, height: 25 }}
                    />
                </Card>
                <div>
                    <Modal isOpen={this.state.modal} className={this.props.className}>
                        <ModalHeader>ข้อมูลที่อยู่สาขา</ModalHeader>
                        <ModalBody>
                            <Select
                                placeholder={"กรุณาระบุชื่อสาขา"}
                                className="branch"
                                options={this.state.branchList}
                                onChange={value => this.handleChangeBranch(value)}
                            />
                            <p style={{ color: "red", textAlign: "center" }}>
                                {this.state.error ? "กรุณาระบุข้อมูลสาขา" : ""}
                            </p>
                            <Progress
                                animated
                                value="100"
                                style={{ display: this.state.style ? "display" : "none" }}
                            />
                            {this.state.address}
                        </ModalBody>
                        <ModalFooter>
                            <Button color="primary" onClick={this.toggle}>
                                เพิ่มสถาที
              </Button>{" "}
                            <Button
                                color="secondary"
                                onClick={() =>
                                    this.setState({
                                        modal: false
                                    })
                                }
                            >
                                ยกเลิก
              </Button>
                        </ModalFooter>
                    </Modal>
                </div>
            </div>
        );
    }
}
export default MapAdmin;

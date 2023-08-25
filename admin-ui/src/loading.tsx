import React from 'react'
import LoadingBar from 'react-top-loading-bar'

class Loading extends React.PureComponent {

    private bar: any

    constructor(props: any) {
        super(props);
        this.bar = React.createRef();
    }

    override componentDidMount(): void {
        this.bar.current.continuousStart()
    }

    override componentWillUnmount(): void {
        this.bar.current.complete()
    }

    render() {

        return (
            <LoadingBar color="#2998ff" ref={this.bar} />
        )
    }
}

export default Loading
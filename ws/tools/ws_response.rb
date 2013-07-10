#
# Standard response for the Webservice
#
class WsResponse

  attr_accessor :code
  attr_accessor :error
  attr_accessor :data

  def initialize
    code = ErrorCodes::OK
  end

  def to_json(*a)
    "{
      \"c\": #{@code}
      \"e\": \"#{@error}\"
      \"r\": #{@data}
    }"
  end

  # Return the json response, encrypted or not
  def to_output

    if $io_encryption
      return Encryption::encrypt(self.to_json)
    else
      return self.to_json
    end
  end

end

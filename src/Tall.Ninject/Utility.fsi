﻿// Copyright (c) Joseph Wasson
// See accompanying LICENSE file for details

module Tall.Utility

  /// Extensions to the Options module
  module Option = begin
    /// Converts the tuple common with many TryXxx methods to an Option
    val fromTryParse : bool * 'a -> 'a option

    /// The opposide of Option.bind, runs the lambda if the option is None
    val elseF : (unit -> 'a option) -> 'a option -> 'a option

    /// Converts a reference value into an option where None represents null.
    val valueToOption : 'a -> 'a option when 'a : null

    /// Converts an otion of 'a into an option of obj.
    val ofObj : 'a option -> obj option
  end

  module Seq =
    val headOrNone : 'a seq -> 'a option
